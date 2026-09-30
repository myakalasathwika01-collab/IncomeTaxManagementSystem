using IncomeTaxManagementSystem.Models;

namespace IncomeTaxManagementSystem.ViewModels
{
    public class UserDashboardViewModel
    {
        public User User { get; set; } = null!;
        public TaxDetails? LatestTaxDetails { get; set; }
        public int TotalDocuments { get; set; }
        public Application? LatestApplication { get; set; }

        public bool HasTaxDetails => LatestTaxDetails != null;
        public bool HasDocuments => TotalDocuments > 0;
        public bool HasApplication => LatestApplication != null;

        public bool IsProfileComplete =>
            !string.IsNullOrWhiteSpace(User.FullName) &&
            !string.IsNullOrWhiteSpace(User.PAN) &&
            !string.IsNullOrWhiteSpace(User.Email) &&
            !string.IsNullOrWhiteSpace(User.Mobile) &&
            !string.IsNullOrWhiteSpace(User.Address);

        public string AccountStatus => string.IsNullOrWhiteSpace(User.Status) ? "Active" : User.Status;

        public string TaxStatusText => HasTaxDetails ? "Completed" : "Not Added Yet";

        public string DocumentStatusText => TotalDocuments > 0 ? $"{TotalDocuments} Uploaded" : "0 Uploaded";

        public string ApplicationStatusText => LatestApplication != null ? LatestApplication.Status : "Not Started";

        // Progress step 1 to 7
        public int ProgressStep
        {
            get
            {
                if (LatestApplication != null)
                {
                    if (string.Equals(LatestApplication.Status, "Approved", StringComparison.OrdinalIgnoreCase)) return 7;
                    if (string.Equals(LatestApplication.Status, "Under Review", StringComparison.OrdinalIgnoreCase)) return 6;
                    if (string.Equals(LatestApplication.Status, "Submitted", StringComparison.OrdinalIgnoreCase)) return 5;
                    if (TotalDocuments > 0) return 4;
                    return 3;
                }
                if (TotalDocuments > 0) return 4;
                if (HasTaxDetails) return 3;
                if (IsProfileComplete) return 2;
                return 1;
            }
        }
    }
}
