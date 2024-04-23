namespace WebApplication1.Classes
{
    public class Competency{
        private Competency(string value) { Value = value; }
        public string Value { get; private set; }
        public static Competency Customer { get { return new Competency("customer"); } }
        public static Competency Employee { get { return new Competency("employee"); } }
    }
}
