using PestControlEngine.Libs.Helpers.Structs;
using PestControlEngine.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControlEngine.Sound
{
    public class SoundManager
    {
        private GameInfo gameInfo;

        public SoundManager(GameInfo info)
        {
            gameInfo = info;
        }

        public void PlaySoundRaw(string sound, float volume = 1.0f, float pitch = 0.0f , float pan = 0.0f)
        {
            ContentLoader.GetSound(sound).Play(volume, pitch, pan);
        }
    }
}
