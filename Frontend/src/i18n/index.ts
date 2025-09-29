import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import LanguageDetector from "i18next-browser-languagedetector";

import enUS from "./en-US.json";
import ptBR from "./pt-BR.json";
import nlNL from "./nl-NL.json";
import frFR from "./fr-FR.json";
import deDE from "./de-DE.json";
import esES from "./es-ES.json";
import itIT from "./it-IT.json";
import svSE from "./sv-SE.json";
import trTR from "./tr-TR.json";
import huHU from "./hu-HU.json";
import jaJP from "./ja-JP.json";
import fiFI from "./fi-FI.json";

const resources = {
  "en-US": {
    translation: enUS,
  },
  "pt-BR": {
    translation: ptBR,
  },
  "nl-NL": {
    translation: nlNL,
  },
  "fr-FR": {
    translation: frFR,
  },
  "de-DE": {
    translation: deDE,
  },
  "es-ES": {
    translation: esES,
  },
  "it-IT": {
    translation: itIT,
  },
  "sv-SE": {
    translation: svSE,
  },
  "tr-TR": {
    translation: trTR,
  },
  "hu-HU": {
    translation: huHU,
  },
  "ja-JP": {
    translation: jaJP,
  },
  "fi-FI": {
    translation: fiFI,
  },
};

i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    resources,
    fallbackLng: "en-US",
    lng: "en-US",
    debug: false,
    interpolation: {
      escapeValue: false,
    },
  });

export default i18n;
