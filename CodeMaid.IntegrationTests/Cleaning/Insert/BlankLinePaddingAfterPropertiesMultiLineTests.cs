﻿using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System.Linq;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Insert\Data\BlankLinePaddingAfterPropertiesMultiLine.cs", "Data")]
    [DeploymentItem(@"Cleaning\Insert\Data\BlankLinePaddingAfterPropertiesMultiLine_Cleaned.cs", "Data")]
    public class BlankLinePaddingAfterPropertiesMultiLineTests
    {
        #region Setup

        private static InsertBlankLinePaddingLogic _insertBlankLinePaddingLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _insertBlankLinePaddingLogic = InsertBlankLinePaddingLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_insertBlankLinePaddingLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\BlankLinePaddingAfterPropertiesMultiLine.cs");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TestEnvironment.RemoveFromProject(_projectItem);
        }

        #endregion Setup

        #region Tests

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertBlankLinePaddingAfterPropertiesMultiLine_CleansAsExpected()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterPropertiesMultiLine = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunInsertBlankLinePaddingAfterPropertiesMultiLine, _projectItem, @"Data\BlankLinePaddingAfterPropertiesMultiLine_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertBlankLinePaddingAfterPropertiesMultiLine_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterPropertiesMultiLine = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunInsertBlankLinePaddingAfterPropertiesMultiLine, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertBlankLinePaddingAfterPropertiesMultiLine_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterPropertiesMultiLine = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunInsertBlankLinePaddingAfterPropertiesMultiLine, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunInsertBlankLinePaddingAfterPropertiesMultiLine(Document document)
        {
            var codeItems = TestOperations.CodeModelManager.RetrieveAllCodeItems(document);
            var properties = codeItems.OfType<CodeItemProperty>().ToList();

            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(properties);
        }

        #endregion Helpers
    }
}