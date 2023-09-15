using UnityEngine;
using System.Collections;

public class FXController {

    public float musicBPM = 120;
    public static bool musicSystemEnabled = true;
    public float musicOSC;
	
	void Update () {

	}

    public void SetLevelDefaults() {

        
    }

    // ----------- Color VFXs

    public Color VFXColorExhanger(Color color1, Color color2, float freq) {

        return Color.Lerp(color1, color2, Mathf.Abs(Mathf.Sin(Mathf.PI * (Time.time - 1.0f) * freq)));
    }

    public Color VFXColorOsc(Color color1, Color color2, float oscFactor) {

        return Color.Lerp(color1, color2, MusicOSC(oscFactor));
    }

    float fadeInStep;

    public Color VFXColorFade(Color initColor, Color finalColor, float time) {

        fadeInStep += 1f / time * Time.deltaTime;

        return Color.Lerp(initColor, finalColor, fadeInStep);
    }

    public bool BinaryMusicOSC(float oscFactor) {

        if (MusicOSC(oscFactor) >= 0.97f) {

            return true;

        } else {

            return false;
        }
    }

    /*/ ----------- Oscillator: oscillates between 0 and 1 (after 1 stars on 0), as many 1 as BPM.

    public float ZeroToOneMusicOSC(float oscFactor) {

        float musicOSC = MusicOSC(oscFactor * 0.5f);

        if (BinaryMusicOSC(oscFactor * 0.5f)) {

            return musicOSC;

        } else {

            return 1.0f - musicOSC;
        }
    }*/

    // ----------- Oscillator: oscillates between 0 and 1 (back & forth), as many 1 as BPM.

    public float MusicOSC(float oscFactor) {

        return Mathf.Abs(Mathf.Sin(Mathf.PI * (Time.time - 1.0f) * musicBPM / 60.0f * oscFactor));
    }

    // ----------- Damper: useful for starting bounces after t = 0 and avoiding starting on bounce peaks.

    float damperStep;

    public void SetDamperDefaults() {

        damperStep = 0.0f;
    }

    public float Damper(float secsToDamp) {

        damperStep += Time.deltaTime;

        return Mathf.Lerp(0.0f, 1.0f, damperStep / secsToDamp);
    }
}
