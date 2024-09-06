using System.Collections.Generic;

namespace feedBackMvc.Models
{
    public class KhaoSatNoiTruViewModel
    {
        // List of question groups
        public List<string> NhomCauHoi { get; set; } = new List<string>();

        // List of question details
        public List<QuestionGroup> CauHoi { get; set; } = new List<QuestionGroup>();

        // Inner class to represent a group of questions
        public class QuestionGroup
        {
            public List<string> TieuDeCauHoi { get; set; } = new List<string>();
            public List<string> CauHoi { get; set; } = new List<string>();
        }
    }
}
