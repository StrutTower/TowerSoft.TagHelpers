namespace TowerSoft.TagHelpers.Options {
    /// <summary>
    /// Sets if the modal should be shown in fullscreen mode.
    /// <br />Documentation: <see href="https://getbootstrap.com/docs/5.3/components/modal/#fullscreen-modal" />
    /// </summary>
    public enum ModalFullscreen {
        /// <summary>
        /// Always show in fullscreen.
        /// </summary>
        modal_fullscreen,
        /// <summary>
        /// Only shows in fullscreen under the sm breakpoint.
        /// </summary>
        modal_fullscreen_sm_down,
        /// <summary>
        /// Only shows in fullscreen under the md breakpoint.
        /// </summary>
        modal_fullscreen_md_down,
        /// <summary>
        /// Only shows in fullscreen under the lg breakpoint.
        /// </summary>
        modal_fullscreen_lg_down,
        /// <summary>
        /// Only shows in fullscreen under the xl breakpoint.
        /// </summary>
        modal_fullscreen_xl_down,
        /// <summary>
        /// Only shows in fullscreen under the xxl breakpoint.
        /// </summary>
        modal_fullscreen_xxl_down,
    }
}
