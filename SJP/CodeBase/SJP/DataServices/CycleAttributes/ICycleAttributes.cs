// *********************************************** 
// NAME             : ICycleAttributes.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 19 Apr 2011
// DESCRIPTION  	: Definition of the CycleAttributes class
// ************************************************
                
                
using System;
using System.Collections.Generic;
namespace SJP.Common.DataServices.CycleAttributes
{
    public interface ICycleAttributes
    {
        /// <summary>
        /// Returns an array of all CycleAttribute objects
        /// </summary>
        /// <returns>CycleAttribute[]</returns>
        List<CycleAttribute> GetCycleAttributes();

        /// <summary>
        /// Returns an array of CycleAttribute for the specified CycleAttributeGroup
        /// </summary>
        /// <param name="cycleAttributeType">CycleAttributeType</param>
        /// <param name="cycleAttributeGroup">CycleAttributeGroup</param>
        /// <returns>CycleAttribute[]</returns>
        List<CycleAttribute> GetCycleAttributes(CycleAttributeType cycleAttributeType, CycleAttributeGroup cycleAttributeGroup);

        /// <summary>
        /// Returns an array of CycleAttributes for the specified CycleAttributeGroup. Attributes returned will 
        /// be those attributes which have been defined as specific Cycle Infrastructure. 
        /// Can be used to determine when the Cycle icon is displayed
        /// </summary>
        /// <param name="cycleAttributeGroup">CycleAttributeGroup</param>
        /// <returns>CycleAttribute[]</returns>
        List<CycleAttribute> GetCycleInfrastructureAttributes(CycleAttributeGroup cycleAttributeGroup);

        /// <summary>
        /// Returns an array of CycleAttributes for the specified CycleAttributeGroup. Attributes returned will 
        /// be those attributes which have been defined as Recommended cycle route attributes. 
        /// Can be used to determine when the Cycle (recommended route) icon is displayed
        /// </summary>
        /// <param name="cycleAttributeGroup">CycleAttributeGroup</param>
        /// <returns>CycleAttribute[]</returns>
        List<CycleAttribute> GetCycleRecommendedAttributes(CycleAttributeGroup cycleAttributeGroup);
    }
}
