﻿using System.Collections.Generic;

namespace MyTraining1101Demo.Web.Models.Consent
{
    public class ConsentViewModel : ConsentInputModel
    {
        public string ClientName { get; set; }

        public string ClientUrl { get; set; }

        public string ClientLogoUrl { get; set; }

        public bool AllowRememberConsent { get; set; }

        public IEnumerable<ScopeViewModel> IdentityScopes { get; set; }

        public IEnumerable<ScopeViewModel> ApiScopes { get; set; }
    }
}
