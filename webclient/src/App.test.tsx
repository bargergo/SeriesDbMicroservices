import { render } from '@testing-library/react';

test('passing test', () => {
    expect(1).toBe(1);
});

test('failing test', () => {
    expect(1).toBe(2);
});
