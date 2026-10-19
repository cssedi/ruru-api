import { useEffect, useState } from 'react'
import Landing from './pages/Landing.jsx'
import Auth from './pages/Auth.jsx'
import Tracker from './pages/Tracker.jsx'

// Tiny hash router: #/ , #/login , #/signup , #/app?tab=meals
function parse() {
  const raw = window.location.hash.replace(/^#/, '') || '/'
  const [path, qs = ''] = raw.split('?')
  return { path, query: new URLSearchParams(qs) }
}

export default function App() {
  const [route, setRoute] = useState(parse)

  useEffect(() => {
    const onChange = () => {
      setRoute(parse())
      window.scrollTo(0, 0)
    }
    window.addEventListener('hashchange', onChange)
    return () => window.removeEventListener('hashchange', onChange)
  }, [])

  useEffect(() => {
    const titles = { '/login': 'Log in', '/signup': 'Create account', '/app': 'Today' }
    document.title = titles[route.path] ? `${titles[route.path]} | ruru` : 'ruru | Train and eat on one screen'
  }, [route.path])

  switch (route.path) {
    case '/login':
      return <Auth mode="login" />
    case '/signup':
      return <Auth mode="signup" />
    case '/app':
      return <Tracker tab={route.query.get('tab') === 'meals' ? 'meals' : 'workout'} />
    default:
      return <Landing />
  }
}
