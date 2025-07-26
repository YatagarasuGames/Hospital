using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConcentration : NetworkBehaviour, IConcentrationProvider
{
    [SyncVar] private float _concentration;

    [SerializeField, SyncVar] private float _maxConcentration;
    [SerializeField, SyncVar] private float _minConcentration;

    [SyncVar] private float _nextConcentrationIncrease;
    [SyncVar, SerializeField] private float _timeBetweenConcentrationIncrease;
    [SerializeField] private float _concentrationIncrease;

    private void Awake()
    {
        _concentration = _minConcentration;
    }

    private void Update()
    {
        if(!isOwned) return;
        if(Time.time >= _nextConcentrationIncrease)
        {
            if (isServer) AddConcentration(_concentrationIncrease);
            else CmdAddConcentration(_concentrationIncrease);

        }
    }

    [Server]
    public void AddConcentration(float concentration)
    {
        _concentration = Mathf.Clamp(_concentration += concentration, _minConcentration, _maxConcentration);
        _nextConcentrationIncrease = Time.time + _timeBetweenConcentrationIncrease;
    }

    [Command]
    public void CmdAddConcentration(float concentration)
    {
        AddConcentration(concentration);
    }

    [Server]
    public void RemoveConcentration(float concentration)
    {
        _concentration = Mathf.Clamp(_concentration -= concentration, _minConcentration, _maxConcentration);
    }

    public float GetConcentration() => _concentration;

    public void ChangeConcentration(float delta)
    {
        if (isServer) AddConcentration(delta);
        else CmdAddConcentration(delta);
    }

    [Server]
    public void IncreaseMinConcentration(float multiplier)
    {
        _minConcentration *= multiplier;
    }

    [Server]
    public void DecreaseMinConcentration(float multiplier)
    {
        _minConcentration /= multiplier;
    }
}
