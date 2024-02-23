namespace EmpDeviceTracking.Models
{
    public class Employee
    {
        public  string EmpID { get; set; }
        public  string EmpName { get; set; }
        public  int Site { get; set; }
        public string SiteName { get; set; }
        public  int Dept { get; set; }
        public string DeptName { get; set; }
        public  Boolean Lap { get; set; }
        public  Boolean Charger { get; set; }
        public  Boolean Mouse { get; set; }
        public  Boolean LapBag { get; set; }
        public  Boolean MousePad { get; set; }
        public string? Remarks { get; set; }
        public string HostName { get; set; }

        // Additional properties for display
        //public string LapDisplay => Lap ? "Yes" : "No";
        //public string ChargerDisplay => Charger ? "Yes" : "No";
        //public string MouseDisplay => Mouse ? "Yes" : "No";
        //public string LapBagDisplay => LapBag ? "Yes" : "No";
        //public string MousePadDisplay => MousePad ? "Yes" : "No";
    }
}
