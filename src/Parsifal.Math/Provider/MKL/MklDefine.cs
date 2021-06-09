namespace Parsifal.Math.Provider.MKL
{
    public enum CBLAS_LAYOUT
    {
        CblasRowMajor = 101,
        CblasColMajor = 102
    };
    public enum CBLAS_TRANSPOSE
    {
        CblasNoTrans = 111,
        CblasTrans = 112,
        CblasConjTrans = 113
    };
    public enum CBLAS_UPLO
    {
        CblasUpper = 121,
        CblasLower = 122
    };
    public enum CBLAS_DIAG
    {
        CblasNonUnit = 131,
        CblasUnit = 132
    };
    public enum CBLAS_SIDE
    {
        CblasLeft = 141,
        CblasRight = 142
    };
    public enum CBLAS_STORAGE
    {
        CblasPacked = 151
    };
    public enum CBLAS_IDENTIFIER
    {
        CblasAMatrix = 161,
        CblasBMatrix = 162
    };
    public enum CBLAS_OFFSET
    {
        CblasRowOffset = 171,
        CblasColOffset = 172,
        CblasFixOffset = 173
    };
}
