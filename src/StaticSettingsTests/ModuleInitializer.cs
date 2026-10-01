public static class ModuleInitializer
{
    #region InitializeOutputs

    [ModuleInitializer]
    public static void Init() =>
        VerifyPdfPig.Initialize(PdfPigOutputs.None);

    #endregion
}
