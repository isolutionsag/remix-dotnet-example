export const defaultNS = "general";

export default {
  // This is the list of languages your application supports
  supportedLngs: ["en", "en-gb", "en-us", "de", "de-ch", "de-de"],
  // This is the language you want to use in case
  // if the user language is not in the supportedLngs
  fallbackLng: "en",
  defaultNS,
  // Disabling suspense is recommended
  react: { useSuspense: false },
};
