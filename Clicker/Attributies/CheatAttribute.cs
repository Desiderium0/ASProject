using System.ComponentModel.DataAnnotations;

namespace Clicker.Attributies
{
    public class CheatAttribute : ValidationAttribute
    {
        private string Code = "Cheat";

        public CheatAttribute(string code) { }

        public override bool IsValid(object value)
        {
            if (value != null && value.ToString() == Code)
                return true;

            return false;   
        }
    }
}
