using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AudioClipPostprocessor : AssetPostprocessor
{
    static AudioImporterSampleSettings standalone = new AudioImporterSampleSettings();
    static AudioImporterSampleSettings android = new AudioImporterSampleSettings();
    static AudioImporterSampleSettings ios = new AudioImporterSampleSettings();

    static AudioClipPostprocessor()
    {
        standalone = new AudioImporterSampleSettings();
        standalone.quality = 0.01f;
        standalone.loadType = AudioClipLoadType.CompressedInMemory;
        standalone.sampleRateSetting = AudioSampleRateSetting.OptimizeSampleRate;
        standalone.compressionFormat = AudioCompressionFormat.Vorbis;

        android = new AudioImporterSampleSettings();
        android.quality = 0.01f;
        android.loadType = AudioClipLoadType.CompressedInMemory;
        android.sampleRateSetting = AudioSampleRateSetting.OptimizeSampleRate;
        android.compressionFormat = AudioCompressionFormat.Vorbis;

        ios = new AudioImporterSampleSettings();
        ios.quality = 0.01f;
        ios.loadType = AudioClipLoadType.CompressedInMemory;
        ios.sampleRateSetting = AudioSampleRateSetting.OptimizeSampleRate;
        ios.compressionFormat = AudioCompressionFormat.Vorbis;
    }

    void OnPreprocessAudio()
    {
        var audioImporter = this.assetImporter as AudioImporter;

        audioImporter.forceToMono = false;
        audioImporter.loadInBackground = false;
        audioImporter.preloadAudioData = false;

        audioImporter.SetOverrideSampleSettings("standalone", standalone);
        audioImporter.SetOverrideSampleSettings("android", android);
        audioImporter.SetOverrideSampleSettings("ios", ios);
    }

}
