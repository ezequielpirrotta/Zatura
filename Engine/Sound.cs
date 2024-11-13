using System;
using Tao.Sdl;

public class Sound
{
    // Atributos
    readonly IntPtr pointer;
    public bool isSoundEffect;
    public int volume;
    // Operaciones

    // Constructor a partir de un nombre de fichero
    public Sound(string nombreFichero, bool isSoundEffect, int initialVolume = SdlMixer.MIX_MAX_VOLUME)
    {
        this.isSoundEffect = isSoundEffect;
        this.volume = initialVolume; // Set initial volume
        if (isSoundEffect)
        {
            pointer = SdlMixer.Mix_LoadWAV(nombreFichero);
            SdlMixer.Mix_VolumeChunk(pointer, volume); // Set volume for sound effect
        }
        else
        {
            pointer = SdlMixer.Mix_LoadMUS(nombreFichero);
            SdlMixer.Mix_VolumeMusic(volume); // Set initial volume for music
        }
    }

    // Reproducir una vez
    public void PlayOnce()
    {
        if(isSoundEffect)
        {
            SdlMixer.Mix_PlayChannel(-1, pointer, 0); 
        }
        else
        {
            SdlMixer.Mix_PlayMusic(pointer, 1);
        }
    }

    // Reproducir continuo (musica de fondo)
    public void Play()
    {
        if (!isSoundEffect)
        {
            // Stop any playing music to ensure single instance
            SdlMixer.Mix_HaltMusic();
            SetMusicVolume(volume); // Set volume again before playing
            SdlMixer.Mix_PlayMusic(pointer, -1);
        }
    }
    private void SetMusicVolume(int volume)
    {
        int result = SdlMixer.Mix_VolumeMusic(volume);
    }
    // Cambiar el volumen 
    public void ChangeVolume(int volumeChange)
    {
        int newVolume = volume + volumeChange;
        if (newVolume < 0) newVolume = 0;
        if (newVolume > SdlMixer.MIX_MAX_VOLUME) newVolume = SdlMixer.MIX_MAX_VOLUME;
        
        volume = newVolume;
        if (isSoundEffect)
        {
            SdlMixer.Mix_VolumeChunk(pointer, volume); // Set volume for sound effect
        }
        else
        {
            SetMusicVolume(volume);
        }
    }
    public void ChangeVolume(bool halfVolume)
    {
        if (halfVolume)
        {
            volume = SdlMixer.MIX_MAX_VOLUME / 2;
            if (!isSoundEffect)
            {
                SdlMixer.Mix_VolumeMusic(this.volume);
            }
            else
            {
                SdlMixer.Mix_VolumeChunk(pointer, this.volume);
            }
        }
    }
    // Interrumpir toda la reproducción de sonido
    public void Stop()
    {
        if (isSoundEffect)
        {
            SdlMixer.Mix_HaltChannel(-1); // Stop all sound effects
        }
        else
        {
            SdlMixer.Mix_HaltMusic(); // Stop background music
        }
    }

}
