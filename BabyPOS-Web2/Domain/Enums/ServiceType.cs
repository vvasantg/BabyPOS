namespace BabyPOS_Web2.Domain.Enums
{
    public enum ServiceType
    {
        DineIn,    // ทานที่ร้าน
        Takeaway,  // กลับบ้าน
        Delivery   // ส่ง/กลับบ้าน(ส่ง)
    }

    public static class ServiceTypeExtensions
    {
        public static string ToDisplayName(this ServiceType serviceType)
        {
            return serviceType switch
            {
                ServiceType.DineIn => "ทานที่ร้าน",
                ServiceType.Takeaway => "กลับบ้าน",
                ServiceType.Delivery => "ส่ง",
                _ => "ทานที่ร้าน"
            };
        }

        public static string ToApiValue(this ServiceType serviceType)
        {
            return serviceType switch
            {
                ServiceType.DineIn => "dineIn",
                ServiceType.Takeaway => "takeaway",
                ServiceType.Delivery => "delivery",
                _ => "dineIn"
            };
        }

        public static ServiceType FromApiValue(string apiValue)
        {
            return apiValue?.ToLower() switch
            {
                "dinein" => ServiceType.DineIn,
                "takeaway" => ServiceType.Takeaway,
                "delivery" => ServiceType.Delivery,
                _ => ServiceType.DineIn
            };
        }
    }
}
