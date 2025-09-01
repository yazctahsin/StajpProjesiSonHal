using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StajProjesi.Data;
using StajProjesi.Models;

namespace StajProjesi.Services
{
    public record RecommendationResult(Hospital Hospital, double? DistanceKm, double TotalScore, int CapabilityScore);

    public class HospitalRecommendationService
    {
        private readonly ApplicationDbContext _db;
        private readonly QuestionnaireService _questions;

        public HospitalRecommendationService(ApplicationDbContext db, QuestionnaireService questions)
        {
            _db = db;
            _questions = questions;
        }

        public async Task<List<RecommendationResult>> RecommendAsync(
            Dictionary<string, string> answers,
            double? userLat,
            double? userLng,
            int maxResults = 3)
        {
            var requirement = _questions.MapAnswersToRequirements(answers);
            var hospitals = await _db.Hospitals.AsNoTracking().ToListAsync();

            var results = new List<RecommendationResult>();

            foreach (var h in hospitals)
            {
                // Yetenek puanı
                var capScore = CapabilityScore(requirement.RequiredCapabilities, h.Capabilities);

                // Mesafe
                double? distance = null;
                if (userLat.HasValue && userLng.HasValue && h.Latitude.HasValue && h.Longitude.HasValue)
                {
                    distance = HaversineKm(userLat.Value, userLng.Value, h.Latitude.Value, h.Longitude.Value);
                }

                // Toplam puan = yetenek uyumu - mesafe ağırlığı
                // Not: Mesafe yoksa sadece yetenek puanı
                var total = capScore - (distance.HasValue ? distance.Value * 0.7 : 0);

                results.Add(new RecommendationResult(h, distance, total, capScore));
            }

            return results
                .OrderByDescending(r => r.TotalScore)
                .ThenBy(r => r.DistanceKm ?? double.MaxValue)
                .Take(maxResults)
                .ToList();
        }

        private static int CapabilityScore(HospitalCapabilities required, HospitalCapabilities has)
        {
            if (required == HospitalCapabilities.None)
                return 10; // Hiç şart yoksa düşük sabit puan

            int score = 0;
            foreach (HospitalCapabilities flag in Enum.GetValues(typeof(HospitalCapabilities)))
            {
                if (flag == HospitalCapabilities.None) continue;
                bool need = required.HasFlag(flag);
                bool available = has.HasFlag(flag);

                if (!need) continue;

                if (available) score += 15;   // Gerekli ve var
                else score -= 25;             // Gerekli ama yok => ciddi eksi
            }
            return score;
        }

        private static double HaversineKm(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371.0; // km
            double dLat = ToRad(lat2 - lat1);
            double dLon = ToRad(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;

            static double ToRad(double deg) => deg * Math.PI / 180.0;
        }
    }
}