namespace ChatApp.Client.ApiUtils
{
    public class BooleanResponce
    {
        public bool value { get; set; }
        public string message { get; set; }

        public BooleanResponce(bool value, string message = "")
        {
            this.value = value;
            this.message = message;
        }
    }
}
