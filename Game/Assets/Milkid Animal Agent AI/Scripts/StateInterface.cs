//Copyright(c) 2017, itsMilkid

public interface StateInterface  {

	void UpdateState();
    void ToIdle();
    void ToRest();
    void ToEat();
    void ToWander();
    void ToFlee();
    void ToChase();
    void ToAttack();
    void ToDead();
}
