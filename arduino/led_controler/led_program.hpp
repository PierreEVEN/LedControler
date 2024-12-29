#pragma once

class LedProgram {
public:
  virtual void set_int_value(uint8_t index, uint8_t value) {

  }
  virtual void set_color_value(uint8_t index, uint8_t r, uint8_t g, uint8_t b) {
    
  }
  virtual CRGB get_led_color(uint16_t led_index) = 0;
};


class DefaultLedProgram : public LedProgram {
  
  CRGB color = CRGB(25, 25, 25);

  virtual void set_color_value(uint8_t index, uint8_t r, uint8_t g, uint8_t b) {
    color = CRGB(r, g, b);
  }

  CRGB get_led_color(uint16_t led_index) override {
    return color;
  }
};