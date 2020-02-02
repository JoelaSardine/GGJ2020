using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MiniGameSpawner : Interactable
{
    public MiniGameType game;
    public float argument;
    public GameObject ItemToSpawn;

    public UnityEvent OnFinished;

    private void Start()
    {
        OnFinished.AddListener(SpawnItem);
    }

    public override void Interact(PlayerController player)
    {
        player.miniGame.SetMiniGame(OnFinished, game, argument);
    }

    private void SpawnItem()
    {
        Item item = Instantiate(ItemToSpawn, transform.position - transform.up, Quaternion.identity).GetComponent<Item>();
        item.gameObject.name = ItemToSpawn.name;
        item.rigidbody.AddForce(Vector2.down * 5, ForceMode2D.Impulse);
    }
}
