export function createPageUrl(pageName: string) {
    return '/' + pageName.replace(/ /g, '-');
}

/** Primary app column: mobile-first, widens on md/lg/xl to use larger screens. */
export const APP_SHELL_MAX_WIDTH_CLASS =
    'max-w-lg md:max-w-3xl lg:max-w-5xl xl:max-w-6xl mx-auto';

/** Centered forms, modals, and player UIs (narrower than full shell). */
export const APP_NARROW_MAX_WIDTH_CLASS =
    'w-full max-w-md md:max-w-lg lg:max-w-xl mx-auto';

/** Article reading: comfortable measure; slightly wider on large viewports. */
export const APP_ARTICLE_MAX_WIDTH_CLASS = 'max-w-2xl lg:max-w-3xl mx-auto';