namespace examProj.Dto
{
    public class SubmitExamDto
    {
        public int Ex_ID { get; set; }
        public int St_ID { get; set; }
        public List<SubmitAnswerDto> Answers { get; set; }
    }

}
