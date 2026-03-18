import { WithPrefixPipe } from './with-prefix-pipe';

describe('WithPrefixPipe', () => {
  it('create an instance', () => {
    const pipe = new WithPrefixPipe();
    expect(pipe).toBeTruthy();
  });
});
