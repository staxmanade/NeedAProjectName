// Type definitions for mocha 1.9.0
// Project: http://visionmedia.github.io/mocha/
// Definitions by: Kazi Manzur Rashid <https://github.com/kazimanzurrashid/>
// DefinitelyTyped: https://github.com/borisyankov/DefinitelyTyped

interface Mocha {
    // Setup mocha with the given setting options.
    setup(options: MochaSetupOptions): Mocha;

    //Run tests and invoke `fn()` when complete.
    run(callback?: () => void): void;

    // Set reporter as function
    reporter(reporter: () => void): Mocha;

    // Set reporter, defaults to "dot"
    reporter(reporter: string): Mocha;

    // Enable growl support.
    growl(): Mocha
}

interface MochaSetupOptions {
    //milliseconds to wait before considering a test slow
    slow?: number;

    // timeout in milliseconds
    timeout?: number;

    // ui name "bdd", "tdd", "exports" etc
    ui?: string;

    //array of accepted globals
    globals?: Array<any>;

    // reporter instance (function or string), defaults to `mocha.reporters.Dot`
    reporter?: any;

    // bail on the first test failure
    bail?: Boolean;

    // ignore global leaks
    ignoreLeaks?: Boolean;

    // grep string or regexp to filter tests with
    grep?: any;
}

declare module mocha {
    interface Done {
        (error?: Error): void;
    }
}

declare var describe : {
    (description: string, spec: () => void): void;
    only(description: string, spec: () => void): void;
    skip(description: string, spec: () => void): void;
    timeout(ms: number): void;
}

declare var it: {
    (expectation: string, assertion?: () => void): void;
    (expectation: string, assertion?: (done: mocha.Done) => void): void;
    only(expectation: string, assertion?: () => void): void;
    only(expectation: string, assertion?: (done: mocha.Done) => void): void;
    skip(expectation: string, assertion?: () => void): void;
    skip(expectation: string, assertion?: (done: mocha.Done) => void): void;
    timeout(ms: number): void;
};

declare function before(action: () => void): void;

declare function before(action: (done: mocha.Done) => void): void;

declare function after(action: () => void): void;

declare function after(action: (done: mocha.Done) => void): void;

declare function beforeEach(action: () => void): void;

declare function beforeEach(action: (done: mocha.Done) => void): void;

declare function afterEach(action: () => void): void;

declare function afterEach(action: (done: mocha.Done) => void): void;

