
#include <SoftwareSerial.h>
#include <FastLED.h>
#include "led_program.hpp"

#define rxPin 2 // Broche 11 en tant que RX, à raccorder sur TX du HC-05
#define txPin 3 // Broche 10 en tant que TX, à raccorder sur RX du HC-05
#define ledPin 5

#define NUM_LEDS    359
#define BRIGHTNESS  255
#define LED_TYPE    WS2812B
#define COLOR_ORDER GRB
CRGB leds[NUM_LEDS];

SoftwareSerial mySerial(rxPin, txPin);

LedProgram* current_program = nullptr;
unsigned long last_update_millis = 0;

void setup()
{
  // define pin modes for tx, rx pins:
  pinMode(rxPin, INPUT);
  pinMode(txPin, OUTPUT);
  mySerial.begin(9600);
  Serial.begin(38400);
  Serial.println("begin");
  
  delay(500); // power-up safety delay
  FastLED.addLeds<LED_TYPE, ledPin, COLOR_ORDER>(leds, NUM_LEDS).setCorrection(TypicalLEDStrip);
  FastLED.setBrightness(BRIGHTNESS);

  set_program<UniformColorProgram>();
  udpate_leds();
}

template<typename Program_T>
void set_program() {
  if (current_program)
    delete current_program;
  current_program = new Program_T();
}

void udpate_leds() {
  if (!current_program)
    return;
  current_program->update_leds(NUM_LEDS, leds);
  FastLED.show();
}

String recv_data;

#define MESSAGE_TYPE_SET_MODE 1
#define MESSAGE_TYPE_SET_COLOR 2
#define MESSAGE_TYPE_SET_INT 3

void loop()
{
  while(mySerial.available()) {
    last_update_millis = millis();
    char recv_char = (char)mySerial.read();
    if (recv_char == '\n') {
      if (recv_data.length() < 2 || recv_data[1] != ';') {
        recv_data = "";
        return;
      }
      int step = 0;
      String message_type;
      String message_value;
      String payload;

      //Serial.println(recv_char);

      for (int i = 0; i < recv_data.length(); ++i) {
        if (recv_data[i] == ';')
          ++step;
        else {
          if (step == 0) {
            message_type += recv_data[i];
          }
          else if (step == 1) {
            message_value += recv_data[i];
          }
          else {
            payload += recv_data[i];
          }
        }
      }
      //mySerial.println(recv_data);
      //Serial.println(message_type + " : " + message_value + " : " + payload);

      switch (message_type.toInt()) {
        case MESSAGE_TYPE_SET_MODE:
          Serial.println(String("Set mode ") + message_value);
          switch (message_value.toInt()) {
            case 0:
              set_program<UniformColorProgram>();
              break;
            case 1:
              set_program<PannerProgram>();
              break;
          }
          break;
        case MESSAGE_TYPE_SET_COLOR:
          {
            int channel = 0;
            String r, g, b;
            for (int i = 0; i < payload.length(); ++i) {
              if (payload[i] == ',')
                ++channel;
              else {
                if (channel == 0) {
                  r += payload[i];
                }
                else if (channel == 1) {
                  g += payload[i];
                }
                else if (channel == 2) {
                  b += payload[i];
                }
              }
            }

            Serial.println(String("Set color #") + message_value + " : " + r + "," + g + "," + b);
            if (current_program)
              current_program->set_color_value(message_value.toInt(), r.toInt(), g.toInt(), b.toInt());
          }
          break;
        case MESSAGE_TYPE_SET_INT:
          Serial.println(String("Set int #") + message_value + " : " + payload);
          if (current_program)
            current_program->set_int_value(message_value.toInt(), payload.toInt());
          break;
      }
      udpate_leds();
      recv_data = "";
    }
    else
      recv_data += recv_char;
  }
  if (millis() - last_update_millis > 300)
    udpate_leds();
}
