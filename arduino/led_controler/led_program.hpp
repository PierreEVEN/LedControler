#pragma once

inline float lerp(float a, float b, float f)
{
    return a * (1.0 - f) + (b * f);
}

class LedProgram {
public:
  virtual void set_int_value(uint8_t index, uint8_t value) {

  }
  virtual void set_color_value(uint8_t index, uint8_t r, uint8_t g, uint8_t b) {
    
  }
  virtual void update_leds(uint16_t num_led, CRGB* leds) = 0;
};


class UniformColorProgram : public LedProgram {
  
  CRGB color = CRGB(25, 25, 25);

  virtual void set_color_value(uint8_t index, uint8_t r, uint8_t g, uint8_t b) {
    color = CRGB(r, g, b);
  }

  void update_leds(uint16_t num_led, CRGB* leds) override {
    for(int i = 0; i < num_led; ++i) {
      leds[i] = color;
    }
  }
};


 #define TWO_PI 6.283185307179586476925286766559


class PannerProgram : public LedProgram {
  
  CRGB colorA = CRGB(70, 25, 25);
  CRGB colorB = CRGB(25, 70, 25);

  int speed = 50;
  int spacing = 50;

  virtual void set_int_value(uint8_t index, uint8_t value) {
    if (index == 0)
      speed = value - 128;
    else
      spacing = value;
  }

  virtual void set_color_value(uint8_t index, uint8_t r, uint8_t g, uint8_t b) {
    if (index == 0)
      colorA = CRGB(r, g, b);
    else
      colorB = CRGB(r, g, b);
  }
  
  void update_leds(uint16_t num_led, CRGB* leds) override {
    CHSV a = rgb2hsv_approximate(colorA);
    CHSV b = rgb2hsv_approximate(colorB);
    float t = millis() / 10000.0 * speed;

    for (int p = 0; p < spacing; ++p) {

      float state = sin(t + (float)p / spacing * TWO_PI) * 0.5 + 0.5;
      CRGB col = CRGB(lerp(colorA.r, colorB.r, state), lerp(colorA.g, colorB.g, state), lerp(colorA.b, colorB.b, state));


      for(int offset = p; offset < num_led; offset += spacing) {
        leds[offset] = col;
      }
    }
  }

/*
  CRGB get_led_color(uint16_t led_index) override {
    float state = sin(t + (float)led_index / spacing) * 0.5 + 0.5;

    return CRGB(lerp(colorA.r, colorB.r, state), lerp(colorA.g, colorB.g, state), lerp(colorA.b, colorB.b, state));

    CHSV final_hsv = CHSV(lerp(a.h, b.h, state), lerp(a.s, b.s, state), lerp(a.v, b.v, state));
    CRGB out;
    hsv2rgb_rainbow(final_hsv, out);
    return out;
  }*/
};