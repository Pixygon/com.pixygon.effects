using UnityEngine;

namespace Pixygon.Effects {
    public class FootstepEffects : MonoBehaviour {
        [SerializeField] private AudioClip[] _stoneClips;
        [SerializeField] private AudioClip[] _dirtClips;
        [SerializeField] private AudioClip[] _sandClips;
        [SerializeField] private AudioClip[] _grassClips;
        [SerializeField] private ParticleSystem _footstepDustFX;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private AudioSource _footstepSfx;

        private AudioClip _previousClip;

        private Terrain _terrain;

        private int _posX;
        private int _posZ;
        private float[] _textureValues;

        private void Start() {
            _terrain = Terrain.activeTerrain;
        }

        private bool CheckCurrentTerrain() {
            if (!Physics.Raycast(new Ray(transform.position + (Vector3.up * .5f), Vector3.down), out var hit, 1f,
                    _mask))
                return false;
            var terrain = hit.collider.GetComponent<Terrain>();
            if (terrain == null) return false;
            if (terrain == _terrain)
                return true;
            _terrain = terrain;
            return true;
        }

        private void GetTerrainTexture() {
            if (CheckCurrentTerrain() == false)
                return;

            ConvertPosition(transform.position);
            var aMap = _terrain.terrainData.GetAlphamaps(_posX, _posZ, 1, 1);
            //Debug.Log("Terrain textures: " + t.terrainData.alphamapTextureCount);
            _textureValues = new float[6];
            _textureValues[0] = aMap[0, 0, 0]; //Grass
            _textureValues[1] = aMap[0, 0, 1]; //Dirt
            _textureValues[2] = aMap[0, 0, 2]; //Sand
            _textureValues[3] = aMap[0, 0, 3]; //SlopeSmall
            _textureValues[4] = aMap[0, 0, 4]; //SlopeMedium
            _textureValues[5] = aMap[0, 0, 5]; //SlopeLarge
            //textureValues[6] = aMap[0, 0, 6];
            //textureValues[7] = aMap[0, 0, 7];
            //textureValues[8] = aMap[0, 0, 8];
            //textureValues[9] = aMap[0, 0, 9];
            //textureValues[10] = aMap[0, 0, 10];
            //textureValues[11] = aMap[0, 0, 11];
            //CheckTexture();
            if (_textureValues[0] > 0f) _footstepSfx.PlayOneShot(GetClip(_sandClips), _textureValues[0]);
            if (_textureValues[1] > 0f) _footstepSfx.PlayOneShot(GetClip(_dirtClips), _textureValues[1]);
            if (_textureValues[2] > 0f) _footstepSfx.PlayOneShot(GetClip(_sandClips), _textureValues[2]);
            if (_textureValues[3] > 0f) _footstepSfx.PlayOneShot(GetClip(_stoneClips), _textureValues[3]);
            if (_textureValues[4] > 0f) _footstepSfx.PlayOneShot(GetClip(_stoneClips), _textureValues[4]);
            if (_textureValues[5] > 0f) _footstepSfx.PlayOneShot(GetClip(_stoneClips), _textureValues[5]);
            /*
            if (textureValues[6] > 0f)
                _footstepSFX.PlayOneShot(GetClip(stoneClips), textureValues[6]); //Mountain 1
            if (textureValues[7] > 0f) {
                _footstepSFX.PlayOneShot(GetClip(dirtClips), textureValues[7]);
                Debug.Log("Play 7");
            }
            if (textureValues[8] > 0f) {
                _footstepSFX.PlayOneShot(GetClip(dirtClips), textureValues[8]);
                Debug.Log("Play 8");
            }
            if (textureValues[9] > 0f)
                _footstepSFX.PlayOneShot(GetClip(stoneClips), textureValues[9]); //Mountain 2
            if (textureValues[10] > 0f) {
                _footstepSFX.PlayOneShot(GetClip(dirtClips), textureValues[10]);
                Debug.Log("Play 10");
            }
            if (textureValues[11] > 0f)
                _footstepSFX.PlayOneShot(GetClip(dirtClips), textureValues[11]); //Dirt
            */
        }

        private void ConvertPosition(Vector3 playerPosition) {
            var terrainPosition = playerPosition - _terrain.transform.position;
            var terrainData = _terrain.terrainData;

            var mapPosition = new Vector3
            (terrainPosition.x / terrainData.size.x, 0,
                terrainPosition.z / terrainData.size.z);

            var xCoord = mapPosition.x * terrainData.alphamapWidth;
            var zCoord = mapPosition.z * terrainData.alphamapHeight;

            _posX = (int)xCoord;
            _posZ = (int)zCoord;
        }

        private AudioClip GetClip(AudioClip[] clipArray) {
            var attempts = 3;
            var selectedClip = clipArray[Random.Range(0, clipArray.Length - 1)];
            while (selectedClip == _previousClip && attempts > 0) {
                selectedClip =
                    clipArray[Random.Range(0, clipArray.Length - 1)];
                attempts--;
            }

            _previousClip = selectedClip;
            return selectedClip;

        }

        public void Step() {
            GetTerrainTexture();
            //_footstepSFX.clip = stoneClips[Random.Range(0, stoneClips.Length)];
            //_footstepSFX.pitch = Random.Range(.85f, 1.1f);
            //_footstepSFX.Play();
            _footstepDustFX.Play();
        }
    }
}
