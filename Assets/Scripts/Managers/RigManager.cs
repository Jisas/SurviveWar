using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rig handsIkLayer;
    [SerializeField] private TwoBoneIKConstraint rightHandIk;
    [SerializeField] private TwoBoneIKConstraint leftHandIk;
    [SerializeField] private MultiAimConstraint headIk;
    [SerializeField] private MultiParentConstraint rifleIk;
    [SerializeField] private MultiParentConstraint gunIk;
    [SerializeField] private MultiParentConstraint knifeIk;

    [Header("Controllers")]
    [SerializeField, Range(0, 1)] private float headWeight;
    [Space(10)]

    [SerializeField, Range(0, 1)] private float handsIkWeight;
    [SerializeField, Range(0, 1)] private float rightHandIkWeight;
    [SerializeField, Range(0, 1)] private float leftHandIkWeight;
    [Space(10)]

    [SerializeField, Range(0, 1)] private float rifle_SO1_Weight;
    [SerializeField, Range(0, 1)] private float rifle_SO2_Weight;
    [Space(5)]

    [SerializeField, Range(0, 1)] private float gun_SO1_Weight;
    [SerializeField, Range(0, 1)] private float gun_SO2_Weight;
    [Space(5)]

    [SerializeField, Range(0, 1)] private float knife_SO1_Weight;
    [SerializeField, Range(0, 1)] private float knife_SO2_Weight;
    [Space(10)]

    private WeightedTransformArray rifleSources;
    private WeightedTransformArray gunSources;
    private WeightedTransformArray knifeSources;

    void Start()
    {
        rifleSources = rifleIk.data.sourceObjects;
        gunSources = gunIk.data.sourceObjects;
        knifeSources = knifeIk.data.sourceObjects;
    }

    void Update()
    {
        headIk.weight = headWeight;
        handsIkLayer.weight = handsIkWeight;
        rightHandIk.weight = rightHandIkWeight;
        leftHandIk.weight = leftHandIkWeight; ;

        rifleSources.SetWeight(0, rifle_SO1_Weight);
        rifleSources.SetWeight(1, rifle_SO2_Weight);
        rifleIk.data.sourceObjects = rifleSources;

        gunSources.SetWeight(0, gun_SO1_Weight);
        gunSources.SetWeight(1, gun_SO2_Weight);
        gunIk.data.sourceObjects = gunSources;

        knifeSources.SetWeight(0, knife_SO1_Weight);
        knifeSources.SetWeight(1, knife_SO2_Weight);
        knifeIk.data.sourceObjects = knifeSources;
    }
}
