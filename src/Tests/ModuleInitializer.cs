public static class ModuleInitializer
{
    #region enable

    [ModuleInitializer]
    public static void Init()
    {
        VerifyPdfPig.Initialize();

        #endregion

        VerifyDiffPlex.Initialize();
    }
}