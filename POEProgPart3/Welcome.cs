using System;
using NAudio.Wave;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEProgPart3
{
    public static class Welcome
    {
        private static WaveOutEvent? outputDevice;
        private static AudioFileReader? audioFile;

        public static void PlayWelcome()
        {
            try
            {
                audioFile = new AudioFileReader(@"C:\Users\Stephen\source\repos\POEProgPart3\POEProgPart3\welcome.wav");
                outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
                outputDevice.Play(); // Play audio and keep it running

                MessageBox.Show("Playing welcome sound...", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing welcome audio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}
