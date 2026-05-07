// src/theme/theme.ts
// Theme constants extracted from default_shadcn_theme.css
export const Colors = {
  background: "#ffffff",
  foreground: "#24292f", // approximated from --foreground
  card: "#ffffff",
  cardForeground: "#24292f",
  popover: "#ffffff",
  popoverForeground: "#24292f",
  primary: "#030213",
  primaryForeground: "#ffffff",
  secondary: "#f5f5f5",
  secondaryForeground: "#030213",
  muted: "#ececf0",
  mutedForeground: "#717182",
  accent: "#e9ebef",
  accentForeground: "#030213",
  destructive: "#d4183d",
  destructiveForeground: "#ffffff",
  border: "rgba(0,0,0,0.1)",
  input: "transparent",
  inputBackground: "#f3f3f5",
  switchBackground: "#cbced4",
  radius: 10, // approximate 0.625rem
};

export const Font = {
  size: 16,
  weightMedium: "500",
  weightNormal: "400",
  family: "Inter",
};

export const Spacing = {
  xs: 4,
  sm: 8,
  md: 16,
  lg: 24,
  xl: 32,
};
