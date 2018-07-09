using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MaritimeFlags;
using UnityEngine;
using Rnd = UnityEngine.Random;

/// <summary>
/// On the Subject of Maritime Flags
/// Created by Timwi
/// </summary>
public class MaritimeFlagsModule : MonoBehaviour
{
    public KMBombInfo Bomb;
    public KMBombModule Module;
    public KMAudio Audio;

    public Texture[] Flags;
    public Texture[] Solves;
    public MeshRenderer FlagDisplay;
    public KMSelectable Compass;
    public Transform CompassNeedle;

    private readonly Texture[] _letterFlags = new Texture[26];
    private readonly Texture[] _digitFlags = new Texture[10];
    private readonly Texture[] _repeaterFlags = new Texture[4];

    private static int _moduleIdCounter = 1;
    private int _moduleId;
    private Callsign _callsign;
    private int _compassSolution;
    private Texture[] _flagsOnModule;
    private int _currentFlagIndex;
    private bool _isSolved;
    private int _curCompass;
    private Coroutine _submit;

    private sealed class Callsign
    {
        public string Name;
        public int Bearing;
    }

    private readonly static Callsign[] _callsigns = @"1STMATE=355;2NDMATE=109;3RDMATE=250;ABANDON=308;ADMIRAL=260;ADVANCE=356;AGROUND=236;ALLIDES=28;ANCHORS=346;ATHWART=78;AZIMUTH=265;BAILERS=357;BALLAST=129;BARRACK=23;BEACHED=170;BEACONS=121;BEAMEND=259;BEAMSEA=316;BEARING=12;BEATING=105;BELAYED=297;BERMUDA=107;BOBSTAY=76;BOILERS=190;BOLLARD=258;BONNETS=332;BOOMKIN=29;BOUNDER=99;BOWLINE=73;BRAILED=165;BREADTH=293;BRIDGES=43;BRIGGED=191;BRINGTO=279;BULWARK=202;BUMBOAT=193;BUMPKIN=119;BURTHEN=294;CABOOSE=10;CAPSIZE=194;CAPSTAN=141;CAPTAIN=1;CARAVEL=295;CAREENS=217;CARRACK=241;CARRIER=94;CATBOAT=219;CATHEAD=177;CHAINED=162;CHANNEL=214;CHARLEY=123;CHARTER=228;CITADEL=246;CLEARED=306;CLEATED=290;CLINKER=163;CLIPPER=130;COAMING=198;COASTED=138;CONSORT=324;CONVOYS=301;CORINTH=225;COTCHEL=140;COUNTER=20;CRANZES=229;CREWING=27;CRINGLE=42;CROJACK=13;CRUISER=333;CUTTERS=147;DANDIES=309;DEADRUN=327;DEBUNKS=331;DERRICK=289;DIPPING=134;DISRATE=158;DOGVANE=231;DOLDRUM=238;DOLPHIN=139;DRAUGHT=351;DRIFTER=201;DROGUES=143;DRYDOCK=4;DUNNAGE=137;DUNSELS=287;EARINGS=60;ECHELON=350;EMBAYED=62;ENSIGNS=98;ESCORTS=11;FAIRWAY=86;FALKUSA=243;FANTAIL=178;FARDAGE=133;FATHOMS=49;FENDERS=337;FERRIES=358;FITTING=89;FLANKED=68;FLARING=172;FLATTOP=195;FLEMISH=323;FLOATED=187;FLOORED=262;FLOTSAM=67;FOLDING=15;FOLLOWS=148;FORCING=118;FORWARD=239;FOULIES=145;FOUNDER=330;FRAMING=156;FREIGHT=174;FRIGATE=227;FUNNELS=230;FURLING=204;GALLEON=300;GALLEYS=292;GALLIOT=9;GANGWAY=210;GARBLED=237;GENERAL=354;GEORGES=100;GHOSTED=30;GINPOLE=233;GIVEWAY=74;GONDOLA=85;GRAVING=253;GRIPIES=335;GROUNDS=153;GROWLER=281;GUINEAS=164;GUNDECK=82;GUNPORT=221;GUNWALE=36;HALYARD=325;HAMMOCK=261;HAMPERS=161;HANGARS=343;HARBORS=182;HARBOUR=154;HAULING=65;HAWSERS=51;HEADING=205;HEADSEA=52;HEAVING=298;HERRING=44;HOGGING=189;HOLIDAY=87;HUFFLER=282;INBOARD=53;INIRONS=111;INSHORE=91;INSTAYS=285;INWATER=103;INWAYOF=69;JACKIES=126;JACKTAR=110;JENNIES=61;JETTIES=71;JIGGERS=353;JOGGLES=215;JOLLIES=188;JURYRIG=166;KEELSON=66;KELLETS=41;KICKING=92;KILLICK=277;KITCHEN=235;LANYARD=155;LAYDAYS=122;LAZARET=283;LEEHELM=197;LEESIDE=116;LEEWARD=95;LIBERTY=81;LIGHTER=302;LIZARDS=276;LOADING=169;LOCKERS=180;LOFTING=186;LOLLING=34;LOOKOUT=171;LUBBERS=340;LUFFING=2;LUGGERS=106;LUGSAIL=284;MAEWEST=5;MANOWAR=348;MARCONI=244;MARINER=209;MATELOT=157;MIZZENS=47;MOORING=310;MOUSING=6;NARROWS=97;NIPPERS=179;OFFICER=252;OFFPIER=266;OILSKIN=31;OLDSALT=347;ONBOARD=249;OREBOAT=271;OUTHAUL=77;OUTWARD=125;PAINTER=75;PANTING=254;PARCELS=113;PARLEYS=38;PARRELS=334;PASSAGE=311;PELAGIC=159;PENDANT=274;PENNANT=135;PICKETS=339;PINNACE=149;PINTLES=317;PIRATES=37;PIVOTED=70;PURSERS=108;PURSUED=90;QUARTER=183;QUAYING=257;RABBETS=315;RATLINE=269;REDUCED=329;REEFERS=146;REPAIRS=35;RIGGING=14;RIPRAPS=167;ROMPERS=17;ROWLOCK=26;RUDDERS=286;RUFFLES=39;RUMMAGE=115;SAGGING=33;SAILORS=207;SALTIES=93;SALVORS=345;SAMPANS=79;SAMPSON=319;SCULLED=291;SCUPPER=206;SCUTTLE=58;SEACOCK=117;SEALING=22;SEEKERS=245;SERVING=220;SEXTANT=275;SHELTER=25;SHIPPED=299;SHIPRIG=247;SICKBAY=142;SKIPPER=45;SKYSAIL=84;SLINGED=196;SLIPWAY=83;SNAGGED=50;SNOTTER=199;SPLICED=268;SPLICES=181;SPONSON=19;SPONSOR=234;SPRINGS=273;SQUARES=263;STACKIE=132;STANDON=226;STARTER=59;STATION=359;STEAMER=57;STEERED=213;STEEVES=314;STEWARD=3;STOPPER=242;STOVEIN=185;STOWAGE=203;STRIKES=124;SUNFISH=342;SWIMMIE=307;SYSTEMS=255;TACKING=18;THWARTS=54;TINCLAD=63;TOMPION=270;TONNAGE=175;TOPMAST=305;TOPSAIL=338;TORPEDO=267;TOSSERS=114;TRADING=251;TRAFFIC=102;TRAMPER=150;TRANSOM=313;TRAWLER=127;TRENAIL=21;TRENNEL=173;TRIMMER=322;TROOPER=46;TRUNNEL=349;TUGBOAT=131;TURNTWO=303;UNSHIPS=318;UPBOUND=326;VESSELS=55;VOICING=321;VOYAGER=212;WEATHER=278;WHALERS=7;WHARVES=341;WHELKIE=223;WHISTLE=151;WINCHES=211;WINDAGE=222;WORKING=218;YARDARM=101"
        .Split(';').Select(str => str.Split('=')).Select(arr => new Callsign { Name = arr[0], Bearing = int.Parse(arr[1]) }).ToArray();
    private readonly static string[] _compassDirections = @"N,NNE,NE,ENE,E,ESE,SE,SSE,S,SSW,SW,WSW,W,WNW,NW,NNW".Split(',');

    void Start()
    {
        _moduleId = _moduleIdCounter++;

        foreach (var flag in Flags)
        {
            var str = flag.name.Substring("Flag-".Length);
            if (str.Length == 2)
                _repeaterFlags[str[1] - '1'] = flag;
            else if (str[0] >= '0' && str[0] <= '9')
                _digitFlags[str[0] - '0'] = flag;
            else
                _letterFlags[str[0] - 'A'] = flag;
        }

        _callsign = _callsigns[Rnd.Range(0, _callsigns.Length)];
        var finalBearing = Rnd.Range(0, 360);
        var flagsOnModule = new List<Texture>();
        for (int i = 0; i < _callsign.Name.Length; i++)
        {
            var pos = i == 0 ? -1 : _callsign.Name.LastIndexOf(_callsign.Name[i], i - 1);
            if (pos != -1)
                flagsOnModule.Add(_repeaterFlags[pos]);
            else if (_callsign.Name[i] >= '0' && _callsign.Name[i] <= '9')
                flagsOnModule.Add(_digitFlags[_callsign.Name[i] - '0']);
            else
                flagsOnModule.Add(_letterFlags[_callsign.Name[i] - 'A']);
        }

        var bearingOnModule = ((finalBearing - _callsign.Bearing + 360) % 360).ToString();
        for (int i = 0; i < bearingOnModule.Length; i++)
        {
            var pos = i == 0 ? -1 : bearingOnModule.LastIndexOf(bearingOnModule[i], i - 1);
            if (pos != -1)
                flagsOnModule.Add(_repeaterFlags[pos]);
            else
                flagsOnModule.Add(_digitFlags[bearingOnModule[i] - '0']);
        }

        _currentFlagIndex = Rnd.Range(0, flagsOnModule.Count);
        _flagsOnModule = flagsOnModule.ToArray();
        _curCompass = Rnd.Range(0, 16);
        _isSolved = false;

        _compassSolution = 0;
        var arr = new[] { 12, 34, 57, 79, 102, 124, 147, 169, 192, 214, 237, 259, 282, 304, 327, 349 };
        for (int i = 0; i < arr.Length; i++)
            if (finalBearing < arr[i])
            {
                _compassSolution = i;
                break;
            }

        StartCoroutine(ShowFlags());
        StartCoroutine(AlignCompass());
        Compass.OnInteract = CompassClicked;

        Debug.LogFormat(@"[Maritime Flags #{0}] Callsign in flags: {1}", _moduleId, _callsign.Name);
        Debug.LogFormat(@"[Maritime Flags #{0}] Bearing in flags: {1}", _moduleId, bearingOnModule);
        Debug.LogFormat(@"[Maritime Flags #{0}] Bearing from callsign: {1}", _moduleId, _callsign.Bearing);
        Debug.LogFormat(@"[Maritime Flags #{0}] Solution: {1}", _moduleId, _compassDirections[_compassSolution]);
    }

    private IEnumerator AlignCompass()
    {
        while (!_isSolved)
        {
            CompassNeedle.localRotation = Quaternion.Lerp(CompassNeedle.localRotation, Quaternion.Euler(0, _curCompass * 360 / 16f, 0), 5 * Time.deltaTime);
            yield return null;
        }
    }

    private bool CompassClicked()
    {
        Compass.AddInteractionPunch();
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, CompassNeedle);
        if (_isSolved)
            return false;
        _curCompass = (_curCompass + 1) % 16;
        if (_submit != null)
            StopCoroutine(_submit);
        _submit = StartCoroutine(Submit());
        return false;
    }

    private IEnumerator Submit()
    {
        yield return new WaitForSeconds(4.7f);
        if (_isSolved)
            yield break;
        Debug.LogFormat(@"[Maritime Flags #{0}] Submitted: {1}", _moduleId, _compassDirections[_curCompass]);
        if (_curCompass == _compassSolution)
        {
            Debug.LogFormat(@"[Maritime Flags #{0}] Module passed.", _moduleId);
            Module.HandlePass();
            _isSolved = true;
            FlagDisplay.material.mainTexture = Solves[Rnd.Range(0, Solves.Length)];
            FlagDisplay.transform.localScale = new Vector3(.095f, .095f, .095f);
        }
        else
        {
            Debug.LogFormat(@"[Maritime Flags #{0}] Strike!", _moduleId);
            Module.HandleStrike();
        }
    }

    private IEnumerator ShowFlags()
    {
        while (!_isSolved)
        {
            FlagDisplay.material.mainTexture = _flagsOnModule[_currentFlagIndex];
            _currentFlagIndex = (_currentFlagIndex + 1) % _flagsOnModule.Length;
            yield return new WaitForSeconds(1.7f);
        }
    }
}
