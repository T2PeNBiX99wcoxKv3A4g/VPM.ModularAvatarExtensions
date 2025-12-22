using UnityEditor;

namespace Test.Test
{
    [InitializeOnLoad]
    public static class TestInit
    {
        static TestInit()
        {
            if (SessionState.GetBool("MAEX_DebugMode", false)) return;
            SessionState.SetBool("MAEX_DebugMode", true);
        }
    }
}