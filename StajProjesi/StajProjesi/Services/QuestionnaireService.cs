using System.Collections.Generic;
using StajProjesi.Models;

namespace StajProjesi.Services
{
    public record QuestionOption(string Value, string Text);
    public record Question(string Id, string Text, List<QuestionOption> Options);

    public record RequirementResult(HospitalCapabilities RequiredCapabilities, bool PreferEmergency);

    public class QuestionnaireService
    {
        public List<Question> GetQuestions()
        {
            return new List<Question>
            {
                new("openWound", "Hastada açık yara var mı?", YesNo()),
                new("severeBleeding", "Şiddetli kanama var mı?", YesNo()),
                new("chestPainOrBreath", "Göğüs ağrısı veya nefes darlığı var mı?", YesNo()),
                new("neuroSymptoms", "Bilinç kaybı, şiddetli baş ağrısı veya felç belirtisi var mı?", YesNo()),
                new("highFeverRash", "Yüksek ateş ve döküntü var mı?", YesNo()),
                new("isChild", "Hasta çocuk mu? (0-16)", YesNo()),
                new("pregnancy", "Hamilelik veya doğum belirtileri var mı?", YesNo()),
                new("fracture", "Kırık/çıkık şüphesi var mı?", YesNo()),
                new("oncologyCare", "Onkoloji ile ilgili aktif tedavi/takip var mı?", YesNo()),
                new("mentalHealthCrisis", "Acil psikiyatrik durum şüphesi var mı?", YesNo()),
            };

            static List<QuestionOption> YesNo() => new()
            {
                new QuestionOption("yes", "Evet"),
                new QuestionOption("no", "Hayır")
            };
        }

        // Cevaplara göre gerekli yetenekler + acil tercihini belirle
        public RequirementResult MapAnswersToRequirements(Dictionary<string, string> answers)
        {
            var req = HospitalCapabilities.None;
            var preferEmergency = false;

            bool IsYes(string id) => answers.TryGetValue(id, out var v) && v == "yes";

            if (IsYes("openWound")) req |= HospitalCapabilities.WoundCare;
            if (IsYes("severeBleeding")) { req |= HospitalCapabilities.Trauma | HospitalCapabilities.WoundCare; preferEmergency = true; }
            if (IsYes("chestPainOrBreath")) { req |= HospitalCapabilities.Cardiology; preferEmergency = true; }
            if (IsYes("neuroSymptoms")) { req |= HospitalCapabilities.Neurology; preferEmergency = true; }
            if (IsYes("highFeverRash")) req |= HospitalCapabilities.InfectiousDisease;
            if (IsYes("isChild")) req |= HospitalCapabilities.Pediatrics;
            if (IsYes("pregnancy")) { req |= HospitalCapabilities.Maternity; preferEmergency = true; }
            if (IsYes("fracture")) { req |= HospitalCapabilities.Orthopedics; preferEmergency = true; }
            if (IsYes("oncologyCare")) req |= HospitalCapabilities.Oncology;
            if (IsYes("mentalHealthCrisis")) { req |= HospitalCapabilities.Psychiatry; preferEmergency = true; }

            if (preferEmergency) req |= HospitalCapabilities.Emergency;

            return new RequirementResult(req, preferEmergency);
        }
    }
}