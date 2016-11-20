
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Groups Preferences.
    /// </summary>
    public class GroupsPreferences
    {
        /// <summary>
        /// Sets the GroupType.
        /// </summary>
        public GroupType GroupType { set; get; }

        /// <summary>
        /// If true, each group will be printed on a new page.
        /// </summary>
        public bool ShowOneGroupPerPage { set; get; }

        /// <summary>
        /// Sets the visibility of the each group's main table's header row.
        /// </summary>
        public bool RepeatHeaderRowPerGroup { set; get; }

        /// <summary>
        /// If set to false and GroupType is set to IncludeGroupingColumns, 
        /// only first row of the each group will show the grouping properties values.
        /// If GroupType is set to HideGroupingColumns, this property will be ignored.
        /// </summary>
        public bool ShowGroupingPropertiesInAllRows { set; get; }

        /// <summary>
        /// Spacing before the main table. Its default value is 15f.
        /// </summary>
        public float SpacingBeforeAllGroupsSummary { set; get; }

        /// <summary>
        /// Spacing after the main table.
        /// </summary>
        public float SpacingAfterAllGroupsSummary { set; get; }

        /// <summary>
        /// Starts a new page if there's not enough room for the new group at the end of the page.
        /// Its default value is 150f. Set it to zero to ignore this rule.
        /// </summary>
        public float NewGroupAvailableSpacingThreshold { set; get; }

        /// <summary>
        /// ctor.
        /// </summary>
        public GroupsPreferences()
        {
            SpacingBeforeAllGroupsSummary = 15f;
            NewGroupAvailableSpacingThreshold = 150f;
        }
    }
}