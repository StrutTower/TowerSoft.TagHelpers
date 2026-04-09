namespace TowerSoft.TagHelpers.Options {
    public static class TowerSoftTagHelperSettings {

        public static string InputDefaultClass { get; set; } = "form-control";
        public static string SelectDefaultClass { get; set; } = "form-select";

        public static string FormFieldContainerElement { get; set; } = "div";
        public static string FormFieldContainerClass { get; set; } = "mb-3";

        public static string HrFormFieldContainerElement { get; set; } = "div";
        public static string HrFormFieldContainerClass { get; set; } = "row mb-3";
        public static string HrFormFieldLabelColumnClass { get; set; } = "col-md-4 col-lg-3";
        public static string HrFormFieldInputColumnClass { get; set; } = "col-md-7 col-lg-6";
        public static string HrFormFieldLabelClass { get; set; } = "text-md-end";

        public static string FormFieldHelpElement { get; set; } = "div";
        public static string FormFieldHelpClass { get; set; } = "text-muted ps-3 small";

        public static string CheckboxContainerElement { get; set; } = "div";
        public static string CheckboxContainerClass { get; set; } = "form-check";
        public static string CheckboxInlineContainerClass { get; set; } = "form-check form-check-inline";

        public static string CheckboxInputClass { get; set; } = "form-check-input";
        public static string CheckboxLabelClass { get; set; } = "form-check-label";

        public static bool LabelRequiredAstrixInLabel { get; set; } = false;
        public static string LabelRequiredAstrixClass { get; set; } = "text-danger ps-1";

        public static string SubmitButtonClass { get; set; } = "brn brn-primary";
        public static string SubmitButtonText { get; set; } = "Save";

        public static string BreadcrumbContainerClass { get; set; } = "breadcrumb";
        public static string BreadcrumbItemClass { get; set; } = "breadcrumb-item";
        public static string BreadcrumbActiveClass { get; set; } = "active";

        public static void UsePicoCssSettings() {
            InputDefaultClass = null;
            SelectDefaultClass = null;

            FormFieldContainerElement = "fieldset";
            FormFieldContainerClass = null;

            HrFormFieldContainerElement = "fieldset";
            HrFormFieldContainerClass = "grid form-grid";
            HrFormFieldLabelColumnClass = null;
            HrFormFieldInputColumnClass = null;
            HrFormFieldLabelClass = null;

            FormFieldHelpElement = "small";
            FormFieldHelpClass = null;

            CheckboxContainerElement = null;
            CheckboxContainerElement = null;
            CheckboxInlineContainerClass = "checkbox-inline";

            CheckboxInputClass = null;
            CheckboxLabelClass = null;

            LabelRequiredAstrixInLabel = true;

            SubmitButtonClass = null;

            BreadcrumbContainerClass = null;
            BreadcrumbItemClass = null;
            BreadcrumbActiveClass = null;
        }
    }
}
