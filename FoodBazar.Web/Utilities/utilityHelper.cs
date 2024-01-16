using Newtonsoft.Json;

namespace FoodBazar.Web.Utilities
{
    public static class UtilityHelper
    {

        public static T DeserializeObject<T>(dynamic json)
        {
          return  JsonConvert.DeserializeObject<T>(Convert.ToString(json));
        } 


        
    }
}
