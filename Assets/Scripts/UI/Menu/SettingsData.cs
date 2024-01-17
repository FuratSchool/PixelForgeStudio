public struct SettingsData
{
    public float masterVolume;
    public float effectsVolume;
    public float backgroundVolume;
    public bool fullscreen;
    public bool invertedY;
    public bool invertedX;
    public int sensitivity;
    public int resolutionWidth;
    public int resolutionHeight;
    public string rebinds;

    public SettingsData(
        string rebinds = "",
        int resolutionWidth = 1920,
        int resolutionHeight = 1080,
        float masterVolume = 0, 
        float effectsVolume = 0, 
        float backgroundVolume = 0, 
        bool fullscreen = true, 
        bool invertedY = false, 
        bool invertedX = false,
        int sensitivity = 1
        ) 
    {
        this.masterVolume = masterVolume;
        this.effectsVolume = effectsVolume;
        this.backgroundVolume = backgroundVolume;
        this.resolutionWidth = resolutionWidth;
        this.resolutionHeight = resolutionHeight;
        this.fullscreen = fullscreen;
        this.invertedY = invertedY;
        this.invertedX = invertedX;
        this.sensitivity = sensitivity;
        this.rebinds = rebinds;
    }
}
