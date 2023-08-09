using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public static Player Instance { get; private set; }
    public event EventHandler onMove;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        onMove?.Invoke(this, EventArgs.Empty);
    }
}
