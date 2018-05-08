﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow
{
    /// <summary>
    /// Helper class to help asserting for CRM Specific objects
    /// </summary>
    public static class AssertHelper
    {
        /// <summary>
        /// Special Assert.AreEqual which can assert EntityReference, OptionSet and Money object. For all other types, the default Assert is called.
        /// </summary>
        /// <param name="actualValue"></param>
        /// <param name="expectedValue"></param>
        public static void AreEqual(object actualValue, object expectedValue)
        {
            if (actualValue == null && expectedValue == null)
                return;

            var type = expectedValue != null ? expectedValue.GetType() : actualValue.GetType();

            if (type == typeof(EntityReference))
                Assert.AreEqual(((EntityReference)expectedValue)?.Id, ((EntityReference)actualValue)?.Id);
            else if (type == typeof(OptionSetValue))
                Assert.AreEqual(((OptionSetValue)expectedValue)?.Value, ((OptionSetValue)actualValue)?.Value);
            else if (type == typeof(Money))
                Assert.AreEqual(((Money)expectedValue)?.Value, ((Money)actualValue)?.Value);
            else
                Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        /// Asserts if an entity has all of the field/value combinations.
        /// </summary>
        /// <param name="record"></param>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        public static void HasProperties(Entity record, Table criteria, CrmTestingContext context)
        {
            foreach (var row in criteria.Rows)
            {
                var actualValue = record.Contains(row[Constants.SpecFlow.TABLE_KEY]) ? record[row[Constants.SpecFlow.TABLE_KEY]] : null;
                var expectedValue = ObjectConverter.ToCrmObject(record.LogicalName, row[Constants.SpecFlow.TABLE_KEY], row[Constants.SpecFlow.TABLE_VALUE], context);

                AreEqual(actualValue, expectedValue);
            }
        }
    }
}
