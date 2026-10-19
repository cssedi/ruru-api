import { useState } from 'react'
import { Logo, ThemeToggle, SteppedBand } from '../components.jsx'

const COPY = {
  login: {
    greet: 'Sawubona.',
    line: 'Your sets and meals are where you left them.',
    title: 'Log in',
    submit: 'Log in',
    busy: 'Logging in…',
    swap: ['New to ruru?', 'Create an account', '#/signup'],
  },
  signup: {
    greet: 'Welcome to ruru.',
    line: 'Set up takes a minute. Your first workout takes longer.',
    title: 'Create your account',
    submit: 'Create account',
    busy: 'Creating account…',
    swap: ['Already have an account?', 'Log in', '#/login'],
  },
}

export default function Auth({ mode }) {
  const c = COPY[mode]
  const [showPw, setShowPw] = useState(false)
  const [busy, setBusy] = useState(false)
  const [errors, setErrors] = useState({})

  function onSubmit(e) {
    e.preventDefault()
    const form = e.currentTarget
    const data = new FormData(form)
    const next = {}
    if (mode === 'signup' && !String(data.get('name')).trim()) next.name = 'Enter your name so we know what to call you.'
    if (!/^\S+@\S+\.\S+$/.test(String(data.get('email')))) next.email = 'Enter an email address like thandi@example.com.'
    if (String(data.get('password')).length < 8) next.password = 'Use at least 8 characters.'
    setErrors(next)
    const first = Object.keys(next)[0]
    if (first) {
      form.elements[first]?.focus()
      return
    }
    // No backend: pretend, then go to the tracker.
    setBusy(true)
    setTimeout(() => { window.location.hash = '#/app' }, 700)
  }

  const err = (k) =>
    errors[k] ? <p className="field-error" id={`${k}-err`}>{errors[k]}</p> : null
  const described = (k) => (errors[k] ? `${k}-err` : undefined)

  return (
    <div className="page auth">
      <aside className="auth-panel shweshwe">
        <Logo />
        <div className="auth-greet">
          <p className="greet">{c.greet}</p>
          <p>{c.line}</p>
        </div>
        <SteppedBand />
      </aside>

      <main className="auth-main">
        <div className="auth-top">
          <a className="link" href="#/">Back to home</a>
          <ThemeToggle />
        </div>
        <form className="auth-form" onSubmit={onSubmit} noValidate>
          <h1>{c.title}</h1>

          {mode === 'signup' && (
            <div className="field">
              <label htmlFor="name">Name</label>
              <input id="name" name="name" autoComplete="name" placeholder="Thandi Mokoena…" aria-invalid={!!errors.name} aria-describedby={described('name')} />
              {err('name')}
            </div>
          )}

          <div className="field">
            <label htmlFor="email">Email</label>
            <input id="email" name="email" type="email" autoComplete="email" spellCheck={false} inputMode="email" placeholder="thandi@example.com" aria-invalid={!!errors.email} aria-describedby={described('email')} />
            {err('email')}
          </div>

          <div className="field">
            <div className="label-row">
              <label htmlFor="password">Password</label>
              {mode === 'login' && <a className="link small" href="#/login">Forgot password?</a>}
            </div>
            <div className="pw-wrap">
              <input id="password" name="password" type={showPw ? 'text' : 'password'} autoComplete={mode === 'login' ? 'current-password' : 'new-password'} aria-invalid={!!errors.password} aria-describedby={described('password') ?? (mode === 'signup' ? 'pw-hint' : undefined)} />
              <button type="button" className="pw-toggle" onClick={() => setShowPw((v) => !v)} aria-pressed={showPw}>
                {showPw ? 'Hide' : 'Show'}
              </button>
            </div>
            {err('password') || (mode === 'signup' && <p className="hint" id="pw-hint">At least 8 characters.</p>)}
          </div>

          {mode === 'signup' && (
            <fieldset className="field goal">
              <legend>Main goal</legend>
              <div className="segmented">
                {['Build muscle', 'Lose fat', 'Maintain'].map((g, i) => (
                  <label key={g}>
                    <input type="radio" name="goal" value={g} defaultChecked={i === 0} />
                    <span>{g}</span>
                  </label>
                ))}
              </div>
            </fieldset>
          )}

          <button className="btn btn-primary btn-block" type="submit" disabled={busy}>
            {busy ? c.busy : c.submit}
          </button>

          <p className="swap">
            {c.swap[0]} <a className="link" href={c.swap[2]}>{c.swap[1]}</a>
          </p>
        </form>
      </main>
    </div>
  )
}
