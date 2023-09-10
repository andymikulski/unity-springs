void Start() {
  spring = new FloatSpring(0.25f, 0.5f);
  spring.SetGoal(someFloat);
}

void Update() {
  spring.Update(Time.deltaTime);
}

void DoThing() {
  var currentValue = spring.Value;
}