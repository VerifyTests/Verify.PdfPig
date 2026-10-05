public static class ModuleInitializer
{
    #region InitializeOutputs

    [ModuleInitializer]
    public static void Init()
    {
        VerifyPdfPig.Initialize();

        // For every test: no text, so it is not extracted either
        VerifierSettings.PageText(PageTextPlacement.None);
    }

    #endregion
}
