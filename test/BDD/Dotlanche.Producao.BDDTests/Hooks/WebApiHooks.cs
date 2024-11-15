﻿using Dotlanche.Producao.BDDTests.Setup;

namespace Dotlanche.Producao.BDDTests.Hooks
{
    [Binding]
    public class WebApiHooks
    {
        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext) 
        {
            var webApi = new WebApiFactory();
            featureContext.FeatureContainer.RegisterInstanceAs(webApi, dispose: true);
        }
    }
}
