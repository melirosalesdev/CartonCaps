using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartonCaps.Model
{
    /// <summary>
    /// Root object for the mock JSON database.
    /// </summary>
    public class MockDatabase
    {
        public List<User> Users { get; set; } = new();

        public List<Referral> Referrals { get; set; } = new();
    }
}
