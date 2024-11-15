﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by Reqnroll (https://www.reqnroll.net/).
//      Reqnroll Version:2.0.0.0
//      Reqnroll Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Dotlanche.Producao.BDDTests.Features
{
    using Reqnroll;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Obter fila de pedidos em produção")]
    public partial class ObterFilaDePedidosEmProducaoFeature
    {
        
        private global::Reqnroll.ITestRunner testRunner;
        
        private static string[] featureTags = ((string[])(null));
        
#line 1 "ObterFilaPedidos.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual async System.Threading.Tasks.Task FeatureSetupAsync()
        {
            testRunner = global::Reqnroll.TestRunnerManager.GetTestRunnerForAssembly();
            global::Reqnroll.FeatureInfo featureInfo = new global::Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("pt-BR"), "Features", "Obter fila de pedidos em produção", null, global::Reqnroll.ProgrammingLanguage.CSharp, featureTags);
            await testRunner.OnFeatureStartAsync(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual async System.Threading.Tasks.Task FeatureTearDownAsync()
        {
            await testRunner.OnFeatureEndAsync();
            global::Reqnroll.TestRunnerManager.ReleaseTestRunner(testRunner);
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public async System.Threading.Tasks.Task TestInitializeAsync()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public async System.Threading.Tasks.Task TestTearDownAsync()
        {
            await testRunner.OnScenarioEndAsync();
        }
        
        public void ScenarioInitialize(global::Reqnroll.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public async System.Threading.Tasks.Task ScenarioStartAsync()
        {
            await testRunner.OnScenarioStartAsync();
        }
        
        public async System.Threading.Tasks.Task ScenarioCleanupAsync()
        {
            await testRunner.CollectScenarioErrorsAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Fila de pedidos com pedidos em recebidos, em preparo e prontos")]
        public async System.Threading.Tasks.Task FilaDePedidosComPedidosEmRecebidosEmPreparoEProntos()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Fila de pedidos com pedidos em recebidos, em preparo e prontos", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 3
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
                global::Reqnroll.Table table5 = new global::Reqnroll.Table(new string[] {
                            "pedidoId",
                            "QueueKey",
                            "Status",
                            "CreationTime"});
                table5.AddRow(new string[] {
                            "e516249a-93bc-439a-b004-0bf15e42c3ad",
                            "3",
                            "Recebido",
                            "2024-11-20 13:06:00"});
                table5.AddRow(new string[] {
                            "4ecb63c5-45a3-4171-b3fb-80cbf065c3bb",
                            "2",
                            "EmPreparo",
                            "2024-11-20 13:05:00"});
                table5.AddRow(new string[] {
                            "8abea682-b18c-4a83-8c01-86a8b60c87cc",
                            "6",
                            "EmPreparo",
                            "2024-11-20 13:06:00"});
                table5.AddRow(new string[] {
                            "b645406e-aba7-4206-8c68-6927d31eb13a",
                            "1",
                            "Pronto",
                            "2024-11-20 13:01:00"});
                table5.AddRow(new string[] {
                            "39b66a58-8e2d-42d3-bde3-d16bd5282b6c",
                            "5",
                            "Pronto",
                            "2024-11-20 13:06:00"});
                table5.AddRow(new string[] {
                            "21257157-99de-4675-93a0-b4210821fd8b",
                            "4",
                            "Recebido",
                            "2024-11-20 13:07:00"});
#line 4
 await testRunner.GivenAsync("os seguintes pedidos estão cadastrados:", ((string)(null)), table5, "Dados ");
#line hidden
#line 12
 await testRunner.WhenAsync("eu solicitar a fila de pedidos em producao", ((string)(null)), ((global::Reqnroll.Table)(null)), "Quando ");
#line hidden
                global::Reqnroll.Table table6 = new global::Reqnroll.Table(new string[] {
                            "QueueKey",
                            "Status"});
                table6.AddRow(new string[] {
                            "1",
                            "Pronto"});
                table6.AddRow(new string[] {
                            "5",
                            "Pronto"});
                table6.AddRow(new string[] {
                            "2",
                            "EmPreparo"});
                table6.AddRow(new string[] {
                            "6",
                            "EmPreparo"});
                table6.AddRow(new string[] {
                            "3",
                            "Recebido"});
                table6.AddRow(new string[] {
                            "4",
                            "Recebido"});
#line 13
 await testRunner.ThenAsync("a fila deve ser retornada na seguinte ordem:", ((string)(null)), table6, "Então ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Fila de pedidos deve ignorar pedidos finalizados")]
        public async System.Threading.Tasks.Task FilaDePedidosDeveIgnorarPedidosFinalizados()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Fila de pedidos deve ignorar pedidos finalizados", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 22
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
                global::Reqnroll.Table table7 = new global::Reqnroll.Table(new string[] {
                            "pedidoId",
                            "QueueKey",
                            "Status",
                            "CreationTime"});
                table7.AddRow(new string[] {
                            "d7b4a407-77c9-4d03-aa77-0af01302e7ee",
                            "2",
                            "EmPreparo",
                            "2024-11-20 13:05:00"});
                table7.AddRow(new string[] {
                            "c5b32602-1260-4a0a-8c74-939d1a57a71b",
                            "6",
                            "EmPreparo",
                            "2024-11-20 13:06:00"});
                table7.AddRow(new string[] {
                            "a667b176-d94e-4319-b863-c13efab59744",
                            "1",
                            "Finalizado",
                            "2024-11-20 12:00:00"});
#line 23
 await testRunner.GivenAsync("os seguintes pedidos estão cadastrados:", ((string)(null)), table7, "Dados ");
#line hidden
#line 28
 await testRunner.WhenAsync("eu solicitar a fila de pedidos em producao", ((string)(null)), ((global::Reqnroll.Table)(null)), "Quando ");
#line hidden
                global::Reqnroll.Table table8 = new global::Reqnroll.Table(new string[] {
                            "QueueKey",
                            "Status"});
                table8.AddRow(new string[] {
                            "2",
                            "EmPreparo"});
                table8.AddRow(new string[] {
                            "6",
                            "EmPreparo"});
#line 29
 await testRunner.ThenAsync("a fila deve ser retornada na seguinte ordem:", ((string)(null)), table8, "Então ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Fila de pedidos deve ignorar pedidos cancelados")]
        public async System.Threading.Tasks.Task FilaDePedidosDeveIgnorarPedidosCancelados()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Fila de pedidos deve ignorar pedidos cancelados", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 34
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
                global::Reqnroll.Table table9 = new global::Reqnroll.Table(new string[] {
                            "pedidoId",
                            "QueueKey",
                            "Status",
                            "CreationTime"});
                table9.AddRow(new string[] {
                            "4c50c1cb-df56-41d8-bb68-63f104d10a80",
                            "2",
                            "EmPreparo",
                            "2024-11-20 13:05:00"});
                table9.AddRow(new string[] {
                            "a977e674-6806-496e-8305-39fde8418a48",
                            "6",
                            "EmPreparo",
                            "2024-11-20 13:06:00"});
                table9.AddRow(new string[] {
                            "4cfd47bb-91bb-40f9-939d-25d222d3952c",
                            "2",
                            "Cancelado",
                            "2024-11-20 12:00:00"});
#line 35
 await testRunner.GivenAsync("os seguintes pedidos estão cadastrados:", ((string)(null)), table9, "Dados ");
#line hidden
#line 40
 await testRunner.WhenAsync("eu solicitar a fila de pedidos em producao", ((string)(null)), ((global::Reqnroll.Table)(null)), "Quando ");
#line hidden
                global::Reqnroll.Table table10 = new global::Reqnroll.Table(new string[] {
                            "QueueKey",
                            "Status"});
                table10.AddRow(new string[] {
                            "2",
                            "EmPreparo"});
                table10.AddRow(new string[] {
                            "6",
                            "EmPreparo"});
#line 41
 await testRunner.ThenAsync("a fila deve ser retornada na seguinte ordem:", ((string)(null)), table10, "Então ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Fila de pedidos vazia")]
        public async System.Threading.Tasks.Task FilaDePedidosVazia()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Fila de pedidos vazia", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 46
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
                global::Reqnroll.Table table11 = new global::Reqnroll.Table(new string[] {
                            "pedidoId",
                            "QueueKey",
                            "Status",
                            "CreationTime"});
#line 47
 await testRunner.GivenAsync("os seguintes pedidos estão cadastrados:", ((string)(null)), table11, "Dados ");
#line hidden
#line 49
 await testRunner.WhenAsync("eu solicitar a fila de pedidos em producao", ((string)(null)), ((global::Reqnroll.Table)(null)), "Quando ");
#line hidden
                global::Reqnroll.Table table12 = new global::Reqnroll.Table(new string[] {
                            "QueueKey",
                            "Status"});
#line 50
 await testRunner.ThenAsync("a fila deve ser retornada na seguinte ordem:", ((string)(null)), table12, "Então ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
    }
}
#pragma warning restore
#endregion
