using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoScript : MonoBehaviour
{
    public enum TYPE
    {
        gameclear,
        gameover
    }
    public TYPE type;
    SpriteRenderer r;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<SpriteRenderer>();
        r.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        if(type == TYPE.gameclear)
        {
            if(PlayerController.gameState == PlayerController.GAMESTATE.gameclear)
            {
                r.color = Color.white;
            }
        }
        else if(type == TYPE.gameover)
        {
            if(PlayerController.gameState == PlayerController.GAMESTATE.gameover)
            {
                r.color = Color.white;
            }
        }
    }
}
