// *********************************************** 
// NAME             : CycleAttribute.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 19 Apr 2011
// DESCRIPTION  	: CycleAttribute class, containing information for a cycle attribute
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.DataServices.CycleAttributes
{
    /// <summary>
    /// CycleAttribute class, containing information for a cycle attribute
    /// </summary>
    public class CycleAttribute
    {
        #region Private members

        private int attributeId;
        private CycleAttributeType attributeType;
        private CycleAttributeGroup attributeGroup;
        private CycleAttributeCategory attributeCategory;
        private string attributeResourceName;
        private Int64 attributeMask;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CycleAttribute()
        {
            this.attributeId = 0;
            this.attributeType = CycleAttributeType.None;
            this.attributeGroup = CycleAttributeGroup.None;
            this.attributeCategory = CycleAttributeCategory.None;
            this.attributeResourceName = string.Empty;
            this.attributeMask = 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="attributeId">Cycle attribuute id</param>
        /// <param name="attributeGroup">Cycle attribute group</param>
        /// <param name="attributeCategory">Cycle attribute category</param>
        /// <param name="attributeResourceName">Cycle attribute resource string name</param>
        /// <param name="attributeMask">Cycle attribute mask</param>
        public CycleAttribute(int attributeId, CycleAttributeType attributeType, CycleAttributeGroup attributeGroup, CycleAttributeCategory attributeCategory, string attributeResourceName, Int64 attributeMask)
        {
            this.attributeId = attributeId;
            this.attributeType = attributeType;
            this.attributeGroup = attributeGroup;
            this.attributeCategory = attributeCategory;
            this.attributeResourceName = attributeResourceName;
            this.attributeMask = attributeMask;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Read only. Cycle attribute id
        /// </summary>
        public int CycleAttributeId
        {
            get { return attributeId; }
        }

        /// <summary>
        /// Read only. Cycle attribute type
        /// </summary>
        public CycleAttributeType CycleAttributeType
        {
            get { return attributeType; }
        }

        /// <summary>
        /// Read only. Cycle attribute group
        /// </summary>
        public CycleAttributeGroup CycleAttributeGroup
        {
            get { return attributeGroup; }
        }

        /// <summary>
        /// Read only. Cycle attribute category
        /// </summary>
        public CycleAttributeCategory CycleAttributeCategory
        {
            get { return attributeCategory; }
        }

        /// <summary>
        /// Read only. Cycle attribute resource string name
        /// </summary>
        public string CycleAttributeResourceName
        {
            get { return attributeResourceName; }
        }

        /// <summary>
        /// Read only. Cycle attribute mask
        /// </summary>
        public Int64 CycleAttributeMask
        {
            get { return attributeMask; }
        }
        #endregion
    }
}
