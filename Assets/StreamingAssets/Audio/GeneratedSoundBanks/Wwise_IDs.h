/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID HIT_PLAYER_1 = 4038917348U;
        static const AkUniqueID HIT_PLAYER_2 = 4038917351U;
        static const AkUniqueID HIT_PLAYER_3 = 4038917350U;
        static const AkUniqueID HIT_PLAYER_4 = 4038917345U;
        static const AkUniqueID HITS = 3232033907U;
        static const AkUniqueID PLAY_COOKINGPOT = 3461871211U;
        static const AkUniqueID PLAY_ENVIRONMENTSOUND = 1121580774U;
        static const AkUniqueID PLAYMUSIC = 417627684U;
        static const AkUniqueID SCREAM_PLAYER_1 = 1163358386U;
        static const AkUniqueID SCREAM_PLAYER_2 = 1163358385U;
        static const AkUniqueID SCREAM_PLAYER_3 = 1163358384U;
        static const AkUniqueID SCREAM_PLAYER_4 = 1163358391U;
        static const AkUniqueID STEPS = 1718617278U;
        static const AkUniqueID STOP_COOKINGPOT = 3540947541U;
        static const AkUniqueID STOP_ENVIRONMENTSOUND = 1416616592U;
        static const AkUniqueID STOPMUSIC = 1917263390U;
        static const AkUniqueID SWING = 2386519981U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace MUSIC_STATE
        {
            static const AkUniqueID GROUP = 3826569560U;

            namespace STATE
            {
                static const AkUniqueID INTRO = 1125500713U;
                static const AkUniqueID LEVEL = 2782712965U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace MUSIC_STATE

        namespace SCREAMING
        {
            static const AkUniqueID GROUP = 2036047442U;

            namespace STATE
            {
                static const AkUniqueID IDLE = 1874288895U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID SCREAM = 737767022U;
            } // namespace STATE
        } // namespace SCREAMING

    } // namespace STATES

    namespace SWITCHES
    {
        namespace WEAPON
        {
            static const AkUniqueID GROUP = 3893417221U;

            namespace SWITCH
            {
                static const AkUniqueID KNIFE = 3312069844U;
                static const AkUniqueID SPOON = 4230374780U;
            } // namespace SWITCH
        } // namespace WEAPON

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID DISTANCE = 1240670792U;
        static const AkUniqueID PANNING = 1820302072U;
        static const AkUniqueID WOODPITCHMOD = 512678656U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID GENERAL = 133642231U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID AMBIANCE = 2981377429U;
        static const AkUniqueID CHARACTER = 436743010U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID SOUNDDESIGN = 3081396824U;
        static const AkUniqueID VOICE = 3170124113U;
    } // namespace BUSSES

    namespace AUX_BUSSES
    {
        static const AkUniqueID REVERB = 348963605U;
        static const AkUniqueID ROOM = 2077253480U;
    } // namespace AUX_BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
